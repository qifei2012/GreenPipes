﻿namespace GreenPipes.Agents
{
    public interface IPipeContextAgent<TContext> :
        PipeContextHandle<TContext>,
        IAgent
        where TContext : class, PipeContext
    {
    }
}
