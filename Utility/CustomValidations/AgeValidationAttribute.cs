using System;
using System.ComponentModel.DataAnnotations;

namespace Utility.CustomValidations
{
	public class AgeValidationAttribute : ValidationAttribute
	{
		private readonly int _minimumAge;
		private readonly int _maximumAge;

		public AgeValidationAttribute(int minimumAge = 13, int maximumAge = 120)
		{
			_minimumAge = minimumAge;
			_maximumAge = maximumAge;
		}

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			if (value == null)
				return new ValidationResult("Date of Birth is required.");

			var dateOfBirth = (DateOnly)value;

			// Check if date is in the future
			if (dateOfBirth > DateOnly.FromDateTime(DateTime.Today))
				return new ValidationResult("Date of Birth cannot be in the future.");

			// Calculate age
			var today = DateOnly.FromDateTime(DateTime.Today);
			int age = today.Year - dateOfBirth.Year;

			// Adjust age if birthday hasn't occurred yet this year
			if (dateOfBirth.Month > today.Month || (dateOfBirth.Month == today.Month && dateOfBirth.Day > today.Day))
				age--;

			// Check if user meets minimum age requirement
			if (age < _minimumAge)
				return new ValidationResult($"You must be at least {_minimumAge} years old to register.");

			// Check if user's age is reasonable
			if (age > _maximumAge)
				return new ValidationResult($"Age cannot exceed {_maximumAge} years.");

			return ValidationResult.Success;
		}
	}
}