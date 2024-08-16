namespace EquipmentWatcherMAUI
{
    public class ResponseObject<T> where T : class
    {
        [System.Text.Json.Serialization.JsonPropertyName("success")]
        public bool Success { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("error")]
        public string Error { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("result")]
        public T Result { get; set; }
    }
}
