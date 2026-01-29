using System.Collections.Generic;
using UnityEngine;

namespace Utility
{
    public static class WeightedList
    {
        public struct WeightedOption<T>
        {
            [SerializeField] public T data;
            [SerializeField] public uint weight;
        }

        public static T GetFromList<T>( List<WeightedOption<T>> options )
        {
            if ( options == null || options.Count == 0 )
                return default( T );

            uint totalWeight = 0;
            foreach ( var item in options )
                totalWeight += item.weight;

            int weightSelect = Random.Range( 1, ( int )totalWeight + 1 ); //1 to t
            for ( int i = 0; i < options.Count; i++ )
            {
                weightSelect -= ( int )options[ i ].weight;
                if ( weightSelect <= 0 )
                {
                    return options[ i ].data;
                }
            }

            return default( T );
        }

        public static void SetWeightAt<T>( this List<WeightedOption<T>> options, int index, uint weight )
        {
            WeightedOption<T> replacer = new WeightedOption<T>();

            replacer.data = options[ index ].data;
            replacer.weight = weight;

            options[ index ] = replacer;
        }
    }
}