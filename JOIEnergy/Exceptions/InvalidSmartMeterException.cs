using System;
namespace JOIEnergy.Exceptions;
public class InvalidSmartMeterException(string message) : Exception(message) {}
