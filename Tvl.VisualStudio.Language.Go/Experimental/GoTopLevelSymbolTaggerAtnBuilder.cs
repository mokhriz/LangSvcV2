﻿namespace Tvl.VisualStudio.Language.Go.Experimental
{
    using Tvl.VisualStudio.Language.Parsing.Collections;
    using Tvl.VisualStudio.Language.Parsing.Experimental.Atn;

    /* Stats with only the block rule modified:
     * 
     *   - _reachableStates: Count = 1549
     *   - _reachableTransitions: Count = 15410
     *
     * Stats with rules inlined:
     * 
     *   - _reachableStates: Count = 1559
     *   - _reachableTransitions: Count = 11815
     */
    internal class GoTopLevelSymbolTaggerAtnBuilder : GoReducedAtnBuilder
    {
        protected override Nfa BuildBlockRule()
        {
            // brace matching only
            return Nfa.Sequence(
                Nfa.Match(GoLexer.LBRACE),
                Nfa.Closure(
                    Nfa.Choice(
                        Nfa.MatchComplement(new Interval(GoLexer.LBRACE, 1), new Interval(GoLexer.RBRACE, 1)),
                        Nfa.Rule(Bindings.Block))),
                Nfa.Match(GoLexer.RBRACE));
        }
    }
}
