//------------------------------------------------------------------------------
// <auto-generated>
//     Ce code a été généré à partir d'un modèle.
//
//     Des modifications manuelles apportées à ce fichier peuvent conduire à un comportement inattendu de votre application.
//     Les modifications manuelles apportées à ce fichier sont remplacées si le code est régénéré.
// </auto-generated>
//------------------------------------------------------------------------------

namespace proera
{
    using System;
    using System.Collections.Generic;
    
    public partial class pannes
    {
        public int id { get; set; }
        public string panne { get; set; }
        public Nullable<int> referenceclient { get; set; }
        public Nullable<System.DateTime> date { get; set; }
        public Nullable<System.DateTime> dateresolution { get; set; }
        public Nullable<byte> resolue { get; set; }
    }
}