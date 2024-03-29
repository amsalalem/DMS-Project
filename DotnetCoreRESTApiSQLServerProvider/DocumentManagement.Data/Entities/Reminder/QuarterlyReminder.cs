﻿using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DocumentManagement.Data
{
    public class QuarterlyReminder
    {
        public Guid Id { get; set; }
        public Guid ReminderId { get; set; }
        [ForeignKey("ReminderId")]
        public Reminder Reminder { get; set; }
        public int Day { get; set; }
        public int Month { get; set; }
        public QuarterEnum Quarter { get; set; }
    }
}
