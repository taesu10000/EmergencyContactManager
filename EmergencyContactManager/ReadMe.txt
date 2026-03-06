1. EmergencyContactManager 프로젝트 시작 프로젝트로 설정
2. Nuget 패키지 설치
3. PostgreSQL 설치
4. 설치시 비밀번호 기억
5. 비밀번호 appsettings.json 파일의 connectionString에 Password=*pass* 에 환경에 맞는 비밀번호입력
6. DB Initialize
비주얼 스튜디오 상단에 보기 -> 터미널 클릭
아래 명령어 입력 후 엔터
dotnet ef migrations add InitialCreate --project infrastructure --startup-project EmergencyContactManager


개발 환경 : Visual Studio 2026
DB : PostgreSQL
OpenAPI : https://{ip}:{port}/swagger/