﻿using System;
using System.Collections.Generic;
using Exam.Models;
using Exam.Models.Enums;

namespace PANDA.Models
{
    public class Package
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public double Weight { get; set; }

        public string ShippingAddress { get; set; }

        public Status Status { get; set; }

        public DateTime? EstimatedDeliveryDate { get; set; }

        public int RecipientId { get; set; }

        public User Recipient { get; set; }
    }
}
