# 🎮 동물아, 진정해! (CalmDown-Animals)

<div align="center">

### 🐾 험악해진 동물들을 포획하고 치유하는 전략형 디펜스 게임

[![Unity](https://img.shields.io/badge/Unity-2021+-black?style=flat-square&logo=unity)](https://unity.com/)
[![C#](https://img.shields.io/badge/C%23-10.0-blue?style=flat-square&logo=csharp)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)](LICENSE)

</div>

---

## 📖 프로젝트 개요

**동물아, 진정해!**는 험악해진 동물들을 포획하여 치유하는 전략적 타워 디펜스 게임입니다. 
플레이어는 맵에 직접 길을 개척하여 동물들을 유도하고, 대원들을 배치하여 동물들을 안전하게 포획해야 합니다.

### 🎯 핵심 특징
- 🛤️ **동적 경로 생성**: 정해진 길이 아닌, 직접 길을 개척하여 동물 유도
- 👥 **전략적 대원 배치**: 다양한 장비를 가진 대원들을 효율적으로 배치
- 📏 **사이즈 기반 포획**: 동물의 크기에 맞는 적절한 포획 도구 선택 필요
- 🚫 **타일 관리 시스템**: 과도한 포획으로 인한 타일 차단 메커니즘
- 🎓 **튜토리얼 시스템**: 단계별 학습을 통한 게임 메커니즘 이해

---

## 👥 개발 팀

| 역할 | 인원 | 담당 업무 |
|------|------|-----------|
| 🎨 **기획** | 1명 | 게임 디자인, 밸런싱, 콘텐츠 기획 |
| 💻 **개발** | 1명 | 시스템 구현, UI/UX, 최적화 |

### 📅 개발 기간
**2025년** (진행 중)

---

## 🚀 구현 내용 및 범위

### 🎮 핵심 게임플레이
```
✅ 육각형 타일 기반 맵 시스템
✅ A* 알고리즘을 활용한 동적 경로 탐색
✅ 실시간 대원 배치 및 전투 시스템
✅ 동물 크기별 포획 난이도 시스템
✅ 웨이브 기반 난이도 조절
```

### 🛠️ 시스템 아키텍처

<details>
<summary><b>📂 Scripts 폴더 구조 (클릭하여 펼치기)</b></summary>

#### 🎯 Core (18개 스크립트)
게임의 핵심 관리 시스템
- `GameManager.cs` - 게임 전체 흐름 관리
- `PathFind.cs` - A* 경로 탐색 알고리즘
- `DataTableManager.cs` - 게임 데이터 관리
- `WindowManager.cs` / `PopupManager.cs` - UI 관리
- `SoundManager.cs` - 사운드 시스템
- `SaveDataManager.cs` - 저장/불러오기 기능
- `PoolingManager.cs` - 오브젝트 풀링 최적화
- `TutorialManager.cs` - 튜토리얼 진행 관리

#### 🦁 Creature (4개 스크립트)
동물 및 대원 캐릭터 시스템
- `Enemy.cs` - 동물 AI 및 이동 로직
- `Crew.cs` - 대원 행동 및 전투 시스템
- `EnemyHealth.cs` - 체력 및 피격 관리
- `Hpbar.cs` - UI 체력바 표시

#### 🗂️ DataTables (10개 스크립트)
게임 데이터 테이블 정의
- `AnimalInfoTable.cs` - 동물 정보 데이터
- `CrewTable.cs` - 대원 정보 데이터
- `EquipmentInfo.cs` / `EquipmentType.cs` - 장비 시스템
- `RoundTable.cs` - 라운드별 설정
- `StringTable.cs` - 다국어 지원 테이블

#### 🎨 Tiles (9개 스크립트)
육각형 타일 시스템
- `TileManager.cs` - 타일 전체 관리
- `PathTile.cs` - 경로 타일 구현
- `DrawManager.cs` / `DrawTile.cs` - 타일 그리기 기능
- `ETATile.cs` - 특수 타일 구현
- `InTileAnimal.cs` - 타일 내 동물 추적

#### 🪟 Windows (18개 스크립트)
UI 창 및 팝업 시스템
- **Window**: 게임 내 메인 UI 창
  - `EditorWindow.cs` - 맵 에디터
  - `EmployUnitWindow.cs` - 대원 고용 창
  - `CrewReadyWindow.cs` - 대원 준비 화면
- **PopUp**: 알림 및 정보 팝업
  - `AnimalInfoPopup.cs` - 동물 정보 표시
  - `ScorePopup.cs` - 점수 결과 표시
  - `OptionPopup.cs` - 게임 옵션
- **Layout**: UI 컴포넌트
  - `ButtonUI.cs`, `Book.cs` 등 재사용 가능한 UI 요소

#### 🎓 Tutorials (8개 스크립트)
단계별 튜토리얼 시스템
- `Tutorial.cs` - 튜토리얼 베이스 클래스
- `DrawTileTutorial.cs` - 타일 그리기 튜토리얼
- `EmployCrewTutorial.cs` - 대원 고용 튜토리얼
- `CrewChangeEquipTutorial.cs` - 장비 변경 튜토리얼
- 기타 게임 메커니즘별 튜토리얼

#### 🎬 기타 시스템
- **Spawner** (3개): 적 및 대원 스폰 관리
- **Animation** (1개): IK 애니메이션 시스템
- **SaveDatas** (2개): JSON 기반 저장 시스템
- **Input** (2개): 카메라 드래그 및 줌 제어
- **Effects** (1개): 날씨 및 시각 효과
- **Particle** (1개): 파티클 이펙트 관리

**총 79개의 C# 스크립트**로 구성된 모듈화된 아키텍처

</details>

### 🎨 주요 기능 구현

#### 1️⃣ 경로 생성 시스템
- 육각형 그리드 기반 맵
- 실시간 경로 재계산
- A* 알고리즘 최적화

#### 2️⃣ 대원 관리 시스템
- 다양한 등급의 대원 (CrewRank)
- 장비 변경 및 업그레이드
- 포획 범위 표시 및 전투

#### 3️⃣ 동물 포획 시스템
- 크기별 분류 (Small, Medium, Large)
- 속도 및 난이도 차등 적용
- 웨이브 기반 스폰 시스템

#### 4️⃣ 맵 에디터
- 인게임 타일 배치 도구
- 경로 시각화
- 저장/불러오기 기능

---

## 🏁 마일스톤

### 📌 1주차: 기반 시스템 구축
- ✅ 육각형 타일 맵 시스템 구현
- ✅ A* 경로 탐색 알고리즘 적용
- ✅ 동물 이동 및 AI 시스템
- ✅ 대원 배치 및 기본 전투 시스템
- ✅ 맵 에디터 툴 제작

### 📌 2주차: 콘텐츠 확장
- ✅ 맵 크기 확장 및 다양화
- ✅ 추가 동물 타입 구현 (사이즈별 분류)
- ✅ 다양한 대원 및 장비 추가
- ✅ 사용 불가 타일 시스템 도입
- ✅ 동물 사이즈별 포획 가능 무기 분류
- ✅ 버그 수정 및 안정화

### 📌 3주차: 완성도 향상
- ✅ 튜토리얼 시스템 완성
- ✅ 편의성 기능 추가 (UI/UX 개선)
- ✅ 게임 옵션 메뉴 구현
- ✅ 세이브/로드 시스템 완성
- ✅ 최종 테스트 및 밸런싱

---

## 🎮 게임 플레이 가이드

### 기본 규칙
1. 🛤️ **경로 구축**: 타일을 배치하여 동물들이 이동할 경로 생성
2. 👥 **대원 배치**: 전략적 위치에 대원 배치
3. 🎯 **동물 포획**: 크기에 맞는 장비로 동물 포획
4. ⚠️ **타일 관리**: 과도한 포획 시 타일 차단 주의
5. 🏆 **라운드 클리어**: 모든 동물을 안전하게 포획

### 승리 조건
- 웨이브별 모든 동물 성공적으로 포획
- 충분한 골드 획득으로 다음 라운드 진행

---

## 🛠️ 기술 스택

| 분야 | 기술 |
|------|------|
| 🎮 **엔진** | Unity 2021+ |
| 💻 **언어** | C# 10.0 |
| 🎨 **렌더링** | Universal Render Pipeline (URP) |
| 📦 **패턴** | Object Pooling, Singleton, Observer |
| 🗄️ **데이터** | ScriptableObject, JSON |
| 🎯 **AI** | A* Pathfinding, FSM |

---

## 📊 프로젝트 통계

```
📁 총 스크립트: 79개
📂 주요 모듈: 15개
🎮 구현된 시스템: 10+개
🐾 동물 타입: 다양한 크기 및 속도
👥 대원 등급: 다중 등급 시스템
```

---

## 🎯 향후 계획

- 🌟 추가 동물 및 대원 타입
- 🗺️ 새로운 맵 및 스테이지
- 🎵 사운드 및 BGM 강화
- 🏆 업적 및 리더보드 시스템
- 📱 모바일 최적화

---

## 📄 라이선스

이 프로젝트는 교육 및 포트폴리오 목적으로 제작되었습니다.

---

<div align="center">

### 🌟 Made with ❤️ by 동물아 진정해 Team

**즐거운 게임 되세요!** 🎮✨

</div>
