using System.Collections.Generic;
using System.Numerics;

namespace Algorithms.Sequences;

/// <summary>
///     <para>
///         Tetranacci numbers: a(n) = a(n-1) + a(n-2) + a(n-3) + a(n-4) with a(0) = a(1) = a(2) = a(3) = 1.
///     </para>
///     <para>
///         OEIS: https://oeis.org/A000288.
///     </para>
/// </summary>
public class TetranacciNumbersSequence : ISequence
{
    public IEnumerable<BigInteger> Sequence
    {
        get
        {
            yield return 1;
            yield return 1;
            yield return 1;
            yield return 1;

            var buffer = new[] { new BigInteger(1), new BigInteger(1), new BigInteger(1), new BigInteger(1) };
            while (true)
            {
                var next = buffer[0] + buffer[1] + buffer[2] + buffer[3];
                buffer[0] = buffer[1];
                buffer[1] = buffer[2];
                buffer[2] = buffer[3];
                buffer[3] = next;
                yield return next;
            }
        }
    }
}
