using LevelEndConditions;
using UnityEngine;
using Zenject;

namespace Installers
{
	public class LevelInstaller : MonoInstaller
	{
		[SerializeField]
		private LevelEndChecker _levelEndChecker;

		[SerializeField]
		private FloatingCharacter _bob;

		[SerializeField]
		private Pistol _bobPistol;

		public override void InstallBindings()
		{
			base.Container.Bind<LevelEndChecker>().FromInstance(_levelEndChecker).AsSingle();
			base.Container.Bind<FloatingCharacter>().FromInstance(_bob).AsSingle();
			base.Container.Bind<Pistol>().FromInstance(_bobPistol).AsSingle();
		}
	}
}
