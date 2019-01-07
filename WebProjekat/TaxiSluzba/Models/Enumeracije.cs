using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaxiSluzba.Models
{
    public enum Ocena
    {
        NEOCENJENA = 0,
        VRLO_LOSA,
        LOSA,
        DOBRA,
        VRLO_DOBRA,
        ODLICNA
    };

    public enum Pol
    {
        MUSKI = 0,
        ZENSKI
    };

    public enum TipVozila
    {
        NEBITNO = 0,
        PUTNICKO,
        KOMBI
    };

    public enum Uloga
    {
        DISPECER = 0,
        VOZAC,
        MUSTERIJA
    };

    public enum Status
    {
        KREIRANA_NA_CEKANJU = 0,
        OTKAZANA,
        FORMIRANA,
        OBRADJENA,
        PRIHVACENA,
        NEUSPESNA,
        USPESNA
    };
}