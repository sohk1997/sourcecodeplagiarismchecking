using System;
using System.Collections.Generic;
using System.Text;

namespace Root.Model {
    public class Feature {
        public Guid Id { get; set; }
        public int FeatureId { get; set; }
        public string FeatureCode { get; set; }
        public string FeatureName { get; set; }
    }
}