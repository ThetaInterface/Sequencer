using System;

namespace Sequencer.Exceptions;

public class NotInitializedException(string? message = null) : Exception(message) { }