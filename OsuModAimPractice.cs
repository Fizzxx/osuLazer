// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables;

using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using System;

namespace osu.Game.Rulesets.Osu.Mods
{
    internal class OsuModAimPractice : Mod, IApplicableToScoreProcessor, IApplicableToDrawableHitObjects
    {
        public override string Name => "Aim Practice";

        public override string Acronym => "AP";

        //public override IconUsage Icon => FontAwesome.Solid.ArrowsAltV;

        public override ModType Type => ModType.Fun;

        public override bool Ranked => false;

        public override string Description => "Play better and the circles shrink!";

        public override double ScoreMultiplier => 1;

        protected static float radiusScaleFactor = 1.0f;


        public void ApplyToScoreProcessor(ScoreProcessor scoreProcessor)
        {
            scoreProcessor.Health.ValueChanged += health =>
            {
                radiusScaleFactor = 1.0f / (float)Math.Pow(2, (2.0f * (float)health.NewValue) - 1.0f);
            };
        }

        protected virtual void ApplyCustomState(DrawableHitObject drawable, ArmedState state)
        {
            var h = (OsuHitObject)drawable.HitObject;

            // apply grow effect
            switch (drawable)
            {
                case DrawableSliderHead _:
                case DrawableSliderTail _:
                    // special cases we should *not* be scaling.
                    break;

                case DrawableSlider _:
                case DrawableHitCircle _:
                    {
                        //using (drawable.BeginAbsoluteSequence(h.StartTime - h.TimePreempt, true))
                        drawable.ScaleTo(radiusScaleFactor);//.Then().ScaleTo(2.0f, h.TimePreempt, Easing.OutExpo);
                        break;
                    }
            }
        }

        public void ApplyToDrawableHitObjects(IEnumerable<DrawableHitObject> drawables)
        {
            foreach (var drawable in drawables)
            {
                switch (drawable)
                {
                    case DrawableSpinner _:
                        continue;

                    default:    
                        drawable.ApplyCustomUpdateState += ApplyCustomState;    //Why is radiusScaleFactor equal to its declaration value here, but not in any of the other methods?
                        break;
                }
            }
        }

        public ScoreRank AdjustRank(ScoreRank rank, double accuracy) => rank;

    }
}
