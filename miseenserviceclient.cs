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
    
    public partial class miseenserviceclient
    {
        public int id { get; set; }
        public string numcompteur { get; set; }
        public Nullable<double> indexDePose { get; set; }
        public Nullable<System.DateTime> dateMiseEnService { get; set; }
        public Nullable<int> referenceClient { get; set; }
        public string statut { get; set; }
        public string utilisateur { get; set; }
    
        public virtual clients clients { get; set; }
    }
}
