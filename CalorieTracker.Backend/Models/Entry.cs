using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace CalorieTracker.Backend.Models
{
	/// <summary>
	/// Represents and entry in meal list.
	/// </summary>
	public class Entry
	{
		/// <summary>
		/// Unique identifier.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Date of the entry.
		/// </summary>
		[Required]
		public DateTimeOffset? Date { get; set; }

		/// <summary>
		/// User comment.
		/// </summary>
		[Required]
		public string Comment { get; set; }

		/// <summary>
		/// Amount of calories.
		/// </summary>
		[Required]
		[Range(1, int.MaxValue)]
		public int? Calories { get; set; }

		/// <summary>
		/// User who created the record.
		/// </summary>
		[JsonIgnore]
		public User User { get; set; }
	}
}
