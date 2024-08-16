#include <SPI.h>
#include <PN532_SPI.h>
#include <PN532.h>
#include <WiFi.h>
#include <esp_wifi.h>
#include <HTTPClient.h>

PN532_SPI pn532spi(SPI, 5);
PN532 nfc(pn532spi);

#define YELLOW_LED 13
#define GREEN_LED 12
#define RED_LED 14
#define TONE_PIN 26
#define RESPONSE_LEN 64

uint8_t baseMac[6];
String lastToken = "";
uint8_t responseLength = RESPONSE_LEN;

uint8_t selectApdu[] = {0x00,                                     /* CLA */
                        0xA4,                                     /* INS */
                        0x04,                                     /* P1  */
                        0x00,                                     /* P2  */
                        0x07,                                     /* Length of AID  */
                        0xFF, 0x65, 0x71, 0x77, 0x74, 0x63, 0x68, /* AID defined on Android App */
                        responseLength - 2 /* Le  */};
const char* ssid = "";
const char* password = "";

String apiEndpoint = "http://server.local/api";

void processErrorInteraction() {
  digitalWrite(YELLOW_LED, LOW);
  digitalWrite(RED_LED, HIGH);
  ledcAttachPin(TONE_PIN, 0);
  ledcWriteNote(0, NOTE_C, 4);
  delay(200);
  ledcDetachPin(TONE_PIN);
  digitalWrite(RED_LED, LOW);
}

void processSuccessInteraction(){
  Serial.print("Token: "); Serial.println(lastToken);
  digitalWrite(YELLOW_LED, HIGH);
  sendInteraction();
}

void successResult() {
  digitalWrite(GREEN_LED, HIGH);
  digitalWrite(RED_LED, LOW);
  digitalWrite(YELLOW_LED, LOW);
  ledcAttachPin(TONE_PIN, 0);
  ledcWriteNote(0, NOTE_A, 4);
  delay(1000);
  ledcDetachPin(TONE_PIN);
  digitalWrite(GREEN_LED, LOW);
}

void errorResult() {
  digitalWrite(GREEN_LED, LOW);
  digitalWrite(RED_LED, HIGH);
  digitalWrite(YELLOW_LED, LOW);
  ledcAttachPin(TONE_PIN, 0);
  ledcWriteNote(0, NOTE_F, 4);
  delay(500);
  ledcDetachPin(TONE_PIN);
  digitalWrite(RED_LED, LOW);
}

void sendInteraction() {
  HTTPClient http;
  http.begin((apiEndpoint + "/Interaction").c_str());
  http.addHeader("Content-Type", "application/json");
  String body =
    "{\"token\": \"" +
    lastToken +
    "\", \"macAddress\": [" +
    String(baseMac[0]) +
    ", " +
    String(baseMac[1]) +
    ", " +
    String(baseMac[2]) +
    ", " +
    String(baseMac[3]) +
    ", " +
    String(baseMac[4]) +
    ", " +
    String(baseMac[5]) +
    "]}";
  int httpResponseCode = http.POST(body);
  Serial.println("Post request body: " + body);
  if (httpResponseCode == 200) {
    Serial.print("HTTP Response code: "); Serial.println(httpResponseCode);
    String payload = http.getString();
    Serial.println(payload);
    successResult();
  }
  else {
    Serial.print("Error code: "); Serial.println(httpResponseCode);
    if (httpResponseCode > 0) {
      Serial.println(http.getString());
    }
    errorResult();
  }
  http.end();
}

void setup() {
  Serial.begin(115200);
  pinMode(YELLOW_LED, OUTPUT);
  pinMode(GREEN_LED, OUTPUT);
  pinMode(RED_LED, OUTPUT);
  ledcSetup(0, 1000, 8);
  ledcAttachPin(TONE_PIN, 0);
  ledcWriteNote(0, NOTE_C, 5);
  delay(500);
  ledcDetachPin(TONE_PIN);
  WiFi.begin(ssid, password);
  esp_wifi_get_mac(WIFI_IF_STA, baseMac);
  Serial.printf("%02x:%02x:%02x:%02x:%02x:%02x\n",
                  baseMac[0], baseMac[1], baseMac[2],
                  baseMac[3], baseMac[4], baseMac[5]);
  Serial.print("Connecting");
  while(WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("");
  Serial.print("Connected to WiFi network with IP Address: ");
  Serial.println(WiFi.localIP());
  setupNFC();
}

void loop() {
  bool success;
  Serial.println("Waiting for an ISO14443A card");
  success = nfc.inListPassiveTarget();

  if (success) {
    Serial.println("Found something!");

    uint8_t response[RESPONSE_LEN];
    responseLength = RESPONSE_LEN;

    success = nfc.inDataExchange(selectApdu, sizeof(selectApdu), response, &responseLength);

    if (success) {
      Serial.print("responseLength: ");
      Serial.println(responseLength);

      printResponse(response, responseLength);
      uint8_t SW1 = response[responseLength - 2];
      uint8_t SW2 = response[responseLength - 1];
      // SW1 0x6X - is commonly warning/error
      if (SW1 & 0x60){
        processErrorInteraction();
      }
      // SW1 0x9X - is success
      else if (SW1 & 0x90){
        lastToken = "";
        for (int i = 0; i < responseLength - 2; i++){
          lastToken += (char)response[i];
        }
        processSuccessInteraction();
      }
    }
    else {
      processErrorInteraction();
      Serial.println("Failed sending SELECT AID");
    }
  }
  else {
    Serial.println("Didn't find anything!");
  }
  delay(1000);
}

void printResponse(uint8_t *response, uint8_t responseLength) {
  String respBuffer;

  for (int i = 0; i < responseLength; i++) {
    uint8_t resp = response[i];
    respBuffer += resp < 0x10 ? " 0" : " "; //Adds leading zeros if hex value is smaller than 0x10
    respBuffer += String(resp, HEX);
  }

  Serial.print("response:");
  Serial.println(respBuffer);
}

void setupNFC() {
  nfc.begin();

  uint32_t versiondata = nfc.getFirmwareVersion();
  if (!versiondata) {
    Serial.print("Didn't find PN53x board");
    while (1); // halt
  }

  // Got ok data, print it out!
  Serial.print("Found chip PN5");
  Serial.println((versiondata >> 24) & 0xFF, HEX);
  Serial.print("Firmware ver. ");
  Serial.print((versiondata >> 16) & 0xFF, DEC);
  Serial.print('.');
  Serial.println((versiondata >> 8) & 0xFF, DEC);

  nfc.SAMConfig();
}
