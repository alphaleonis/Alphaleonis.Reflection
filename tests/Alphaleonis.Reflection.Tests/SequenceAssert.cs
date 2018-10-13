// Copyright (c) Peter Palotas
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.CodeDom.Compiler;

namespace Tests.Alphaleonis.Reflection
{
   internal static class SequenceAssert
   {
      public static void AreEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, string message = null)
      {
         AreEquivalent<T>(expected, actual, EqualityComparer<T>.Default, message);
      }

      public static void AreEquivalent<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer, string message = null)
      {
         if (expected == null)
         {
            Assert.AreEqual(expected, actual);
         }
         else if (actual == null)
         {
            Assert.Fail($"{(message == null ? "" : message + "\r\n")}The sequences are not equal, expected a sequence but found <null>.");
         }
         else
         {
            T[] expectedArray = expected.ToArray();
            T[] actualArray = actual.ToArray();

            if (expectedArray.Length != actualArray.Length)
               Assert.Fail($"{(message == null ? "" : message + "\r\n")}The sequences are not equivalent; Expected a sequence of length {expectedArray.Length}, but got a sequence of length {actualArray.Length}.\r\nExpected:\r\n{GetDisplayTextForObject(expectedArray)}\r\nActual:\r\n{GetDisplayTextForObject(actualArray)}");

            IEnumerable<IGrouping<T, T>> expectedGrouped = expectedArray.GroupBy(item => item, comparer);

            foreach (IGrouping<T, T> group in expectedGrouped)
            {
               int actualCount = actualArray.Count(item => comparer.Equals(item, group.Key));
               int expectedCount = group.Count();

               if (actualCount != expectedCount)
               {
                  string format = $"{(message == null ? "" : message + "\r\n")}The sequences are not equivalent; Expected {expectedCount} occurrences of item <{group.Key}>, but got {actualCount} occurrences of that item.\r\nExpected:\r\n{GetDisplayTextForObject(expectedArray)}\r\nActual:\r\n{GetDisplayTextForObject(actualArray)}";
                  Assert.Fail(format);
               }
            }
         }
         
      }

      public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual)
      {
         IEqualityComparer<T> comparer = EqualityComparer<T>.Default;
         AreEqual<T>(expected, actual, comparer);
      }

      public static void AreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T> comparer)
      {
         if (expected == null)
         {
            Assert.AreEqual(expected, actual);
         }
         else if (actual == null)
         {
            Assert.Fail("The sequences are not equal, expected a sequence but found <null>.");
         }
         else
         {
            T[] expectedArray = expected.ToArray();
            T[] actualArray = actual.ToArray();

            if (expectedArray.Length != actualArray.Length)
               Assert.Fail("The sequences are not equal; Expected a sequence of length {0}, but got a sequence of length {1}.", expectedArray.Length, actualArray.Length);

            for (int i = 0; i < expectedArray.Length; i++)
            {
               if (!comparer.Equals(expectedArray[i], actualArray[i]))
                  Assert.Fail("The sequences are not equal; First difference encountered at position {0}; expected <{1}> but got <{2}>.", i, expectedArray[i], actualArray[i]);
            }
         }
      }

      public static void IsSubSet<T>(IEnumerable<T> expectedSuperSet, IEnumerable<T> actualSubSet)
      {
         IsSubSet<T>(expectedSuperSet, actualSubSet, EqualityComparer<T>.Default);
      }

      public static void IsSubSet<T>(IEnumerable<T> expectedSuperSet, IEnumerable<T> actualSubSet, IEqualityComparer<T> comparer)
      {
         if (expectedSuperSet == null)
            throw new ArgumentNullException("expectedSuperSet", "expectedSuperSet is null.");

         if (actualSubSet == null)
         {
            Assert.Fail("The sequence is not a subset of the specified list, it was null!");
         }
         else
         {
            T[] expected = expectedSuperSet.ToArray();
            T[] actual = actualSubSet.ToArray();

            for (int i = 0; i < actual.Length; i++)
            {
               if (!expected.Any(e => comparer.Equals(e, actual[i])))
               {
                  Assert.Fail(String.Format("The sequence:\r\n[{0}]\r\nis not a subset of the expected sequence:\r\n[{1}]\r\nThe element {2} at position {3} is not part of the expected superset.",
                     SequenceToString(actual),
                     SequenceToString(expected),
                     (object)actual[i] ?? "<null>", i));
               }
            }
         }

      }

      public static void Contains<T>(IEnumerable<T> sequence, T element)
      {
         Contains<T>(sequence, element, EqualityComparer<T>.Default);
      }

      public static void Contains<T>(IEnumerable<T> sequence, T element, IEqualityComparer<T> comparer)
      {
         foreach (var item in sequence)
         {
            if (comparer.Equals(item, element))
               return;
         }

         Assert.Fail("The sequence did not contain the expected element: {0}", element);
      }

      public static void DoesNotContain<T>(IEnumerable<T> sequence, T element)
      {
         DoesNotContain<T>(sequence, element, EqualityComparer<T>.Default);
      }

      public static void DoesNotContain<T>(IEnumerable<T> sequence, T element, IEqualityComparer<T> comparer)
      {
         foreach (var item in sequence)
         {
            if (comparer.Equals(item, element))
               Assert.Fail("The sequence contained the element: {0}, but it was not expected to.", element);
         }
      }

      private static string SequenceToString<T>(IEnumerable<T> sequence)
      {
         if (sequence == null)
            return "<null>";

         const int MAX_LENGTH = 30;
         StringBuilder sb = new StringBuilder();
         sb.AppendLine("[");
         int i = 0;
         foreach (T element in sequence)
         {
            if (i != 0)
               sb.Append(", ");

            if (i > MAX_LENGTH)
            {
               sb.Append("...");
               break;
            }

            i++;

            if (element == null)
               sb.Append("<null>");
            else
               sb.Append(element);
         }

         sb.Append("]");
         return sb.ToString();
      }

      public static void IsEmpty<T>(IEnumerable<T> sequence)
      {
         if (sequence.Any())
            Assert.Fail("The sequence provided is not empty.");
      }

      private static string GetDisplayTextForObject(object value)
      {
         StringBuilder sb = new StringBuilder();
         using (IndentedTextWriter writer = new IndentedTextWriter(new StringWriter(sb)))
         {
            AppendValueToString(writer, value, true);
            return sb.ToString();
         }
      }

      private static void AppendValueToString(IndentedTextWriter sb, object value, bool qouteString = false)
      {
         if (sb == null)
            throw new ArgumentNullException("sb", "sb is null.");

         if (value == null)
         {
            sb.Write("null");
         }
         else if (value is string)
         {
            if (qouteString) sb.Write('\"');
            sb.Write(value);
            if (qouteString) sb.Write('\"');
         }
         else if (value is System.Collections.IEnumerable)
         {
            sb.WriteLine("[");
            sb.Indent++;
            bool isFirst = true;
            foreach (object v in ((System.Collections.IEnumerable)value))
            {
               if (!isFirst)
               {
                  sb.WriteLine(",");
               }
               else
               {
                  isFirst = false;
               }

               AppendValueToString(sb, v);

            }
            sb.Indent--;
            sb.WriteLine();
            sb.Write(']');
         }
         else
         {
            sb.Write(value.ToString());
         }
      }
   }
}
