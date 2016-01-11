namespace GoldBoxExplorer.Lib.Plugins.Dax
{
    public class RenderBlockFactory
    {
        public IRenderBlock CreateUsing(FileBlockParameters parameters)
        {
            if (new EgaSpriteBlockSpecification().IsSatisfiedBy(parameters))
                return new EgaSpriteBlock(parameters);

            if (new VgaStrataBlockSpecification().IsSatisfiedBy(parameters))
                return new VgaStrataBlock(parameters);

            if (new VgaSpriteBlockSpecification().IsSatisfiedBy(parameters))
                return new VgaSpriteBlock(parameters);

            if (new VgaMixedBlockSpecification().IsSatisfiedBy(parameters))
                return new VgaMixedBlock(parameters);

            if (new VgaBlockSpecification().IsSatisfiedBy(parameters))
                return new VgaBlock(parameters);

            if (new EgaBlockSpecification().IsSatisfiedBy(parameters))
                return new EgaBlock(parameters);

            if (new MonoBlockSpecification().IsSatisfiedBy(parameters))
                return new MonoBlock(parameters);

            return new UnknownBlock();
        }
    }
}