using System;
using System.Collections.Generic;

namespace BE.Models
{
    public partial class EventPostNotification
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int NotificationId { get; set; }

        public virtual Notification Notification { get; set; }
        public virtual Event Sender { get; set; }
    }
}
