﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using 鮮蔬果季_前台.Models;

namespace 鮮蔬果季_前台.ViewModels
{
    public class EventListViewModel
    {
        public Event _Event = null;
        public Event Event
        {
            get
            {
                if (_Event == null)
                    _Event = new Event();
                return _Event;
            }
            set
            {
                _Event = value;
            }
        }


        public List <EventPhotoBank> _EventPhotoBank = null;
        public  List<EventPhotoBank> EventPhoto
        {
            get
            {
                if (_EventPhotoBank == null)
                    _EventPhotoBank = new List<EventPhotoBank>();
                return _EventPhotoBank;
            }
            set
            {
                _EventPhotoBank = value;
            }
        }

        public City _City = null;
        public City City
        {
            get
            {
                if (_City == null)
                    _City = new City();
                return _City;
            }
            set
            {
                _City = value;
            }
        }


        public EventRegistration _EventRegistration = null;
        public EventRegistration EventRegistration
        {
            get
            {
                if (_EventRegistration == null)
                    _EventRegistration = new EventRegistration();
                return _EventRegistration;
            }
            set
            {
                _EventRegistration = value;
            }
        }

        public Supplier _Supplier = null;
        public Supplier Supplier
        {
            get
            {
                if (_Supplier == null)
                    _Supplier = new Supplier();
                return _Supplier;
            }
            set
            {
                _Supplier = value;
            }
        }

    }
}
