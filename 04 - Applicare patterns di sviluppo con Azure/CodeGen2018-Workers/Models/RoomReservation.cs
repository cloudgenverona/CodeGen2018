using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace CodeGen2018_Workers.Models
{
    public class RoomReservation: TableEntity
    {
        public string Title { get; set; }
        public string UserId { get; set; }
        public decimal Length { get; set; }
    }
}
