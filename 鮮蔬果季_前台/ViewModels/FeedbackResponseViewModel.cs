using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    
    public class FeedbackResponseViewModel
    {
        public FeedbackResponse _feedbackResponse = null;
        public FeedbackResponse feedbackResponse {
            get {
                if (_feedbackResponse == null)
                    _feedbackResponse = new FeedbackResponse();
                return _feedbackResponse;
            }
            set {
                _feedbackResponse = value;
            }
        }
        public Feedback _feedback = null;
        public Feedback feedback
        {
            get
            {
                if (_feedback == null)
                    _feedback = new Feedback();
                return _feedback;
            }
            set
            {
                _feedback = value;
            }
        }
        public Product _product = null;
        public Product product
        {

            get
            {
                if (_product == null)
                    _product = new Product();
                return _product;
            }
            set
            {
                _product = value;
            }
        }
        public Order _order = null;
        public Order order 
        {

            get
            {
                if (_order == null)
                    _order = new Order();
                return _order;
            }
            set
            {
                _order = value;
            }
        }
        public Supplier _supplier = null;
        public Supplier supplier
        {

            get
            {
                if (_supplier == null)
                    _supplier = new Supplier();
                return _supplier;
            }
            set
            {
                _supplier = value;
            }
        }












        [DisplayName("回應項目")]
        public string FeedbackName { get { return this.feedback.FeedbackName; } set {this.feedback.FeedbackName=value; } }
        public int FeedbackResponseId { get; set; }

        public int OrderDetailId { get; set; }

        public int FeedbackId { get; set; }

        public string FeedbackComment { get; set; }
    }
}
