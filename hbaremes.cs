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
    
    public partial class hbaremes
    {
        public int Id { get; set; }
        public Nullable<double> Puissance { get; set; }
        public Nullable<double> Montant { get; set; }
        public Nullable<int> NiveauPuissance { get; set; }
        public Nullable<int> Calibre2 { get; set; }
        public Nullable<int> branch { get; set; }
        public Nullable<int> Usage { get; set; }
    
        public virtual hbranches hbranches { get; set; }
        public virtual hcalibres hcalibres { get; set; }
        public virtual usages usages { get; set; }
        public virtual hnivpuissances hnivpuissances { get; set; }
    }
}
