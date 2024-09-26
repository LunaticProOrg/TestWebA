using UnityEngine;
using Zenject;

public class AnalyticsInstaller : MonoInstaller
{
    [SerializeField]
    private string serverUrl; 

    public override void InstallBindings()
    {
        Container.Bind<IHttpClient>().To<UnityHttpClient>().AsSingle();
        Container.Bind<IEventStorage>().To<PlayerPrefsEventStorage>().AsSingle();

        Container.Bind<IEventService>()
            .To<EventService>()
            .AsSingle()
            .WithArguments(serverUrl, Container.Resolve<IEventStorage>(), Container.Resolve<IHttpClient>(), 2f);
    }
}
