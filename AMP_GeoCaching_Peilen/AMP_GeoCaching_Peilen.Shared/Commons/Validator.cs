﻿using System;
using System.Windows;
using System.Collections.ObjectModel;
using AMP.GeoCachingTools.Model;
using System.ComponentModel;
using System.Linq;
using Windows.UI.Xaml;
using System.Collections.Generic;
using AMP.GeoCachingTools.Commons;
using System.Text.RegularExpressions;
using AMP.GeoCachingTools.ViewModel;

namespace AMP.GeoCachingTools.Commons
{
    public class Validator : BaseINPC
    {

        private GeoCoordinate initialCoordinate { get; set; }

        private string distance { get; set; }

        private string direction { get; set; }

        private List<string> values = new List<string>();

        public List<string> emptyFields { get; set; }

        public Validator(GeoCoordinate initialCoordinate, string distance, string direction)
        {
            this.initialCoordinate = initialCoordinate;
            this.distance = distance;
            this.direction = direction;
        }

        public Validator(GeoCoordinate initialCoordinate)
        {
            values.Add(initialCoordinate.LongitudeCoordinate);
            values.Add(initialCoordinate.LatitudeCoordinate);
        }
        
        // Check and parse values
        public double validateDoubleValue(string value)
        {
            double doubleValue;

            Double.TryParse(value, out doubleValue);

            return doubleValue;
        }

        // Simple check, if fields are empty
        public bool checkEmptyTextboxes()
        {
            bool isEmpty = false;

            emptyFields = new List<string>();

            if (distance == null || distance.Equals(""))
            {
                emptyFields.Add("Entfernung");
                isEmpty = true;
            }

            if (direction == null || direction.Equals(""))
            {
                emptyFields.Add("Richtung");
                isEmpty = true;
            }

            if (initialCoordinate.LatitudeCoordinate == null || initialCoordinate.LatitudeCoordinate.Equals(""))
            {
                emptyFields.Add("Breitengrad");
                isEmpty = true;
            }

            if (initialCoordinate.LongitudeCoordinate == null || initialCoordinate.LongitudeCoordinate.Equals(""))
            {
                emptyFields.Add("Längengrad");
                isEmpty = true;
            }

            return isEmpty;
        }

        // direction between 0° and 360°
        public bool isInDirectionRange(double value)
        {
            bool isInRange = false;

            if (value >= 0 && value <= 360)
            {
                isInRange = true;
            }

            return isInRange;
        }

        // Main validate method which delegate to submethods with pattern matching
        public bool validateFormat(string format)
        {
            bool isInFormat = true;

            foreach (string value in values)
            {
                if (format == Constants.DegreesMinutes)
                {
                    if (!validateDegreesMinutesFormat(value))
                    {
                        isInFormat = false;
                        break;
                    }
                }
                else if (format == Constants.Degrees)
                {
                    if (!validateDegreesFormat(value))
                    {
                        isInFormat = false;
                        break;
                    }
                }
                else if (format == Constants.DegreesMinutesSeconds)
                {
                    if (!validateDegreesMinutesSecondsFormat(value))
                    {
                        isInFormat = false;
                        break;
                    }
                }
            }

            return isInFormat;
        }

        // Matches for '52', '0', '12.12', but not for '00', '52.1.1', '52.ghgh', 'gfhgfh' ...
        private bool validateDegreesFormat(string value)
        {
            return Regex.IsMatch(value, @"^\-?([1-9]\d*|0)(\.\d*)?$");
        }

        // Matches for '52 13.00', '52 13', '52', but not for '00', '52.1.1', '52.ghgh', '52 dsg' ...
        private bool validateDegreesMinutesFormat(string value)
        {
            return Regex.IsMatch(value, @"^\-?([1-9]\d*|0)( \-?([1-9]\d*|0)(\.\d*)?)?$");
        }

        // Matches for '52 563 52.45', '0', '52 13.00', but not for '00', '52.1.1', '52.ghgh', 'gfhgfh' ...
        private bool validateDegreesMinutesSecondsFormat(string value)
        {
            return Regex.IsMatch(value, @"^\-?([1-9]\d*|0)( \-?([1-9]\d*|0))?( \-?([1-9]\d*|0)(\.\d*)?)?$");
        }
    }
}
