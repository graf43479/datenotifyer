﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    public class ExceptionDetail
    {
        [Key]
        public int Id { get; set; }
        public string ExceptionMessage { get; set; }    // сообщение об исключении
        public string ControllerName { get; set; }      // контроллер, где возникло исключение
        public string ActionName { get; set; }          // действие, где возникло исключение
        public string StackTrace { get; set; }          // стек исключения
        public DateTime Date { get; set; }              // дата и время исключения
    }   
}
