using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using eTerm.AsyncSDK.Base;

namespace ASync.WeatherPlugIn {

    [Serializable]
    internal class WeatherResult : BaseBinary<WeatherResult> {
        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>The day.</value>
        public string Day { get; set; }

        /// <summary>
        /// Gets or sets the week day.
        /// </summary>
        /// <value>The week day.</value>
        public string WeekDay { get; set; }

        /// <summary>
        /// Gets or sets the name of the report.
        /// </summary>
        /// <value>The name of the report.</value>
        public string ReportName { get; set; }

        /// <summary>
        /// Gets or sets the report result.
        /// </summary>
        /// <value>The report result.</value>
        public string ReportResult { get; set; }

        /// <summary>
        /// Gets or sets the wind direction.
        /// </summary>
        /// <value>The wind direction.</value>
        public string WindDirection { get; set; }

        /// <summary>
        /// Gets or sets the min temperature.
        /// </summary>
        /// <value>The min temperature.</value>
        public string MinTemperature { get; set; }

        /// <summary>
        /// Gets or sets the max temperature.
        /// </summary>
        /// <value>The max temperature.</value>
        public string MaxTemperature { get; set; }

        /// <summary>
        /// Gets or sets the wind power.
        /// </summary>
        /// <value>The wind power.</value>
        public string WindPower { get; set; }
    }

    [Serializable]
    public sealed class WeaterCityVersion : BaseBinary<WeaterCityVersion> {
        /// <summary>
        /// Gets or sets the version date.
        /// </summary>
        /// <value>The version date.</value>
        public DateTime? VersionDate { get; set; }

        /// <summary>
        /// Gets or sets the web city list.
        /// </summary>
        /// <value>The web city list.</value>
        public List<WeatherCity> WebCityList { get; set; }
    }

    [Serializable]
    public sealed class WeatherCity : BaseBinary<WeatherCity> {
        /// <summary>
        /// Gets or sets the city id.
        /// </summary>
        /// <value>The city id.</value>
        public string CityId { get; set; }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <value>The name of the city.</value>
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets the city pin yin.
        /// </summary>
        /// <value>The city pin yin.</value>
        public string CityPinYin { get; set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>The parent id.</value>
        public string ParentId { get; set; }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj) {
            return this.CityName == (obj as WeatherCity).CityName;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode() {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString() {
            return string.Format(@"{1}--{0}", this.CityName,this.CityId);
        }
    }
}
