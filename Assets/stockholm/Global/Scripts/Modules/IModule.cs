/// <summary>
/// 모듈 인터페이스입니다. ModuleManager에 등록해 관리합니다.
/// </summary>
public interface IModule {
	bool InitModule();
	bool RunModule();
	void StopModule();
	void DestroyModule();
}
