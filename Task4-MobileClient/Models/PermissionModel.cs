﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EquipmentWatcherMAUI.Models
{
    public class PermissionModel
    {
        [JsonPropertyName("id")]
        public int ID { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
