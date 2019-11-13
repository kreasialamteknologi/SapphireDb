﻿using RealtimeDatabase.Attributes;
using System;
using System.Linq;
using Microsoft.AspNetCore.Http;

namespace WebUI.Data.Models
{
    [QueryAuth("Auth")]
    [CreateEvent("BeforeCreate")]
    [UpdateEvent("BeforeUpdate")]
    [QueryFunction("QueryFunction")]
    public class Message : Base
    {
        public Message()
        {
            CreatedOn = DateTime.UtcNow;
        }

        public static Func<Message, bool> QueryFunction(HttpContext context)
        {
            string userId = context.User.Claims.FirstOrDefault(cl => cl.Type == "Id")?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return (m) => false;
            }

            return (m) => m.UserId == userId || m.ToId == userId;
        }

        public bool Auth(HttpContext context)
        {
            string userId = context.User.Claims.FirstOrDefault(cl => cl.Type == "Id")?.Value;
            return !string.IsNullOrEmpty(userId) && (UserId == userId || ToId == userId);
        }

        public void BeforeCreate(HttpContext context)
        {
            UserId = context.User.Claims.FirstOrDefault(cl => cl.Type == "Id")?.Value;
        }

        public void BeforeUpdate(HttpContext context)
        {
            UpdatedOn = DateTime.UtcNow;
        }

        public string UserId { get; set; }

        public string ToId { get; set; }

        public string Content { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime UpdatedOn { get; set; }
    }
}
