// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Completed by Rachel Saya for CS 3500, September 2015

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// Infrastructure to handle dependencies.
    /// (s1,t1) is an ordered pair of strings
    /// s1 depends on t1 --> t1 must be evaluated before s1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// (Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.)
    /// </summary>
    public class DependencyGraph
    {
        private Dictionary<String, HashSet<String>> dependentGraph = new Dictionary<String, HashSet<String>>();
        
        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            dependentGraph= new Dictionary<String, HashSet<String>>();

        }

        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size {
            get {
                int i = 0;                                              
                foreach (var elements in dependentGraph.Values) {        //For all elements in dependentGraph     
                    foreach (var element in elements) {                   //Count each ordered pair in elements
                    
                        i++;
                }
                    }
                return i;
            }
        }

        /// <summary>
        /// The size of dependees(s).
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string s]
        {
            get {
                int i = 0;
                foreach (var pair in dependentGraph) {                   //For every set in dependent Graph
                    HashSet<String> dependents = pair.Value;
                    if (dependents.Contains(s)) {                        //Count how many dependents contain s
                        i++;
                    }
                }
                return i;
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            foreach (var elements in dependentGraph) {
                foreach (var element in elements.Value) { 
                    return true;                                        //Checks to see if there is at least one 
                }                                                       //dependent to return true, otherwise false
            }
            return false;
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            foreach (var element in dependentGraph) { 
                if (element.Value.Contains(s)) { 
                    return true;                                        //Checks to see if there is at least one 
                }                                                       //dependee to return true, otherwise false
            }
            return false;
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string s)
        {
            foreach (var element in dependentGraph) {
                if (element.Key.Contains(s)) {
                    foreach (var item in element.Value) {
                      yield return item;                                //Statement to return each element one at a time.
                    }
                }
            }
        }


        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string s)
        {
            foreach (var elements in dependentGraph) {
                if (elements.Value.Contains(s)) {
                    yield return elements.Key;                           //Statement to return each element one at a time.
                }
            }
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   s depends on t
        ///
        /// </summary>
        /// <param name="s"> s cannot be evaluated until t is</param>
        /// <param name="t"> t must be evaluated first.  S depends on T</param>
        public void AddDependency(string s, string t)
        {
            if (!dependentGraph.ContainsKey(s)) {                         //If s is not in dependentGraph
                dependentGraph.Add(s, new HashSet<String> {{t}});         //Add s and t
            }
            else { 
                dependentGraph[s].Add(t);                                   //Otherwise, add t as a dependent to s
            }
        }                                                                   


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            if (dependentGraph.ContainsKey(s))                              //If it is there, remove it
            {
                if(dependentGraph[s].Contains(t))
                    dependentGraph[s].Remove(t);
            }

        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t).
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            if (dependentGraph.ContainsKey(s)) {
                dependentGraph.Remove(s);                                   //Removes all old s dependents
            }
            foreach (string t in newDependents) {                           //Replaces dependents with new dependents
                AddDependency(s, t);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s).
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            foreach (KeyValuePair<String, HashSet<String>> keyValue in dependentGraph) {
                if (keyValue.Value.Contains(s)) {                           //Removes all s dependees
                    keyValue.Value.Remove(s);
                }
            }
            foreach (string t in newDependees) {                            //Replaces dependees with new dependees
                AddDependency(t, s);
            }
        }

    }
}