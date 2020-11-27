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
    
    public partial class clients
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public clients()
        {
            this.migration = new HashSet<migration>();
            this.miseenserviceclient = new HashSet<miseenserviceclient>();
            this.reclamation = new HashSet<reclamation>();
            this.releves = new HashSet<releves>();
            this.resiliation = new HashSet<resiliation>();
        }
    
        public int ID { get; set; }
        public string Nom1 { get; set; }
        public string Prenom { get; set; }
        public string Num_ID { get; set; }
        public string Tel { get; set; }
        public string Raison_Social { get; set; }
        public string Date_Abonnement { get; set; }
        public string Village { get; set; }
        public string Niv_Service { get; set; }
        public string Type_Elect { get; set; }
        public string calibre { get; set; }
        public int Reference_Contrat { get; set; }
        public Nullable<double> Montant_Encaisse { get; set; }
        public Nullable<int> Etat_Client { get; set; }
        public string Commentaire { get; set; }
        public string Date_Mise_en_Service { get; set; }
        public string Date_Resiliation { get; set; }
        public string X_GPS { get; set; }
        public string Y_GPS { get; set; }
        public Nullable<int> Bordereau { get; set; }
        public Nullable<int> Contrat { get; set; }
        public string numcompteur { get; set; }
        public string Ancien_indexe { get; set; }
        public double SoldeTotal { get; set; }
        public string IDPayment { get; set; }
        public string Prev_Bill { get; set; }
        public string Last_Bill { get; set; }
        public string NbrEP { get; set; }
        public string Modifié { get; set; }
        public string Créé { get; set; }
        public string Créé_par { get; set; }
        public string periodeFacturee { get; set; }
        public string dateCoupure { get; set; }
        public string dateAbonn { get; set; }
        public string dateMeS { get; set; }
        public string idlastbill { get; set; }
        public Nullable<int> NivPuissance { get; set; }
        public Nullable<int> activite { get; set; }
        public string sqlstate { get; set; }
        public string refclient { get; set; }
        public Nullable<int> codevillage { get; set; }
        public Nullable<int> modePaiement { get; set; }
        public string numeropaiement { get; set; }
        public string TypeBranch { get; set; }
        public string etat { get; set; }
        public string usage { get; set; }
    
        public virtual hnivpuissances hnivpuissances { get; set; }
        public virtual raisonsociale raisonsociale { get; set; }
        public virtual typepaiement typepaiement { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<migration> migration { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<miseenserviceclient> miseenserviceclient { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<reclamation> reclamation { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<releves> releves { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<resiliation> resiliation { get; set; }
    }
}
