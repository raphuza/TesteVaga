﻿using System.Globalization;

namespace Questao1
{
    class ContaBancaria {

        public int Numero { get; }
        public string Titular { get; set; }
        public double Saldo { get; private set; }
        private const double TaxaSaque = 3.50;

        public ContaBancaria(int numero, string titular, double depositoInicial = 0)
        {
            Numero = numero;
            Titular = titular;
            Saldo = depositoInicial;
        }

        public void Deposito(double valor)
        {
            Saldo += valor;
        }

        public void Saque(double valor)
        {
            Saldo -= (valor + TaxaSaque);
        }

        public override string ToString()
        {
            return $"Conta {Numero}, Titular: {Titular}, Saldo: $ {Saldo.ToString("F2", CultureInfo.InvariantCulture)}";
        }


    }
}
