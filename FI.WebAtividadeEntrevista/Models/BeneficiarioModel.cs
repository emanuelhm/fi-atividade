﻿using System.ComponentModel.DataAnnotations;

namespace WebAtividadeEntrevista.Models
{
    public class BeneficiarioModel
    {
        public long Id { get; set; }

        public string CPF { get; set; }

        public string Nome { get; set; }
    }
}
