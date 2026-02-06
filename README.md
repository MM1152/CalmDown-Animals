<div align="center">

<h2>[2025] CalmDown-Animals 🦁</h2>
<p>유니티 합반 프로젝트<br>
육각형 타일 기반 전략 시뮬레이션 게임<br>
</p>
<img width="413" height="413" alt="Image" src="https://github.com/user-attachments/assets/94a097d8-4a6d-4849-80a3-c794aae77393" />
</div>

---

### 🗂️ 개요

- **인원**: 개발 1인 , 기획 1인
- **프로젝트명**: 진정해, 동물아!
- **빌드**: Mobile ( Android )
- **개발툴**: Unity Engine
- **기간**: 2025-09 ~ 2025-10 ( 3주 )

---

## 📂 Assets/Script 폴더 구조
```
Assets/Scripts/
│
├── 📁 Animation/
│   └── 📄 FrInverseKinematic.cs
│
├── 📁 Core/
│   ├── 📄 DataTable.cs
│   ├── 📄 DataTableManager.cs
│   ├── 📄 Define.cs
│   ├── 📄 ObjectPool.cs
│   ├── 📄 PathFind.cs
│   ├── 📄 PriorityQueue.cs
│   ├── 📄 SafeArea.cs
│   ├── 📄 SaveData.cs
│   ├── 📄 SceneChange.cs
│   ├── 📄 SoundManager.cs
│   └── 📄 WindowManager.cs
│
├── 📁 Creature/
│   ├── 📄 Crew.cs
│   ├── 📄 Enemy.cs
│   ├── 📄 EnemyHealth.cs
│   └── 📄 Hpbar.cs
│
├── 📁 DataTables/
│   ├── 📄 AnimalCRRank.cs
│   ├── 📄 AnimalInfoTable.cs
│   ├── 📄 AnimalSizeTable.cs
│   ├── 📄 AnimalSpeedTable.cs
│   ├── 📄 CrewRankTable.cs
│   ├── 📄 CrewTable.cs
│   ├── 📄 EquipmentInfo.cs
│   ├── 📄 EquipmentType.cs
│   ├── 📄 RoundTable.cs
│   └── 📄 StringTable.cs
│
├── 📁 Debug/
│   └── 📄 DebugMode.cs
│
├── 📁 Effects/
│   └── 📄 Cloudes.cs
│
├── 📁 Input/
│   ├── 📄 DragCamera.cs
│   └── 📄 ZoomCamera.cs
│
├── 📁 Interface/
│   └── 📄 IDamageAble.cs
│
├── 📁 Particle/
│   └── 📄 Particle.cs
│
├── 📁 SaveDatas/
│   └── 📄 JsonConvert.cs
│
├── 📁 Spawner/
│   ├── 📄 ParticleSpawner.cs
│   └── 📄 UndoCrew.cs
│
├── 📁 Test/
│   └── 📄 TestAnimationLigging.cs
│
├── 📁 Tiles/
│   └── 📄 Tile.cs
│
├── 📁 Tutorials/
│   ├── 📄 CreateRoadTutorial.cs
│   ├── 📄 CrewSellingTutorial.cs
│   ├── 📄 DeleteTileTutorial.cs
│   ├── 📄 EmployCrewTutorial.cs
│   ├── 📄 EmployCrewTutorial2.cs
│   └── 📄 Tutorial.cs
│
├── 📁 WIndows/
│   └── 📁 PopUp/
│       └── 📄 MenuTabPopup.cs
│
└── 📄 Weapon.cs
```
## 🔑 주요 코드 파일

### 🗺️ 길찾기 (PathFinding) 관련

| 파일 | 설명 | 링크 |
|------|------|------|
| `PathFind.cs` | A* 알고리즘 기반 경로 탐색 구현 | [📄 보기](https://github.com/MM1152/CalmDown-Animals/blob/main/Assets/Scripts/Core/PathFind.cs) |
| `PriorityQueue.cs` | 우선순위 큐 자료구조 직접 구현 (Min-Heap) | [📄 보기](https://github.com/MM1152/CalmDown-Animals/blob/main/Assets/Scripts/Core/PriorityQueue.cs) |

### 🎲 타일 (Tile) 시스템

| 파일 | 설명 | 링크 |
|------|------|------|
| `Tile.cs` | 타일 베이스 클래스 (타입, 색상 관리) | [📄 보기](https://github.com/MM1152/CalmDown-Animals/blob/main/Assets/Scripts/Tiles/TIle.cs) |
| `PathTile.cs` | A* 경로 탐색용 타일 (G, H, F값, 육각 거리 계산) | [📄 보기](https://github.com/MM1152/CalmDown-Animals/blob/main/Assets/Scripts/Tiles/PathTile.cs) |
| `DrawTile.cs` | 맵 에디터용 드로잉 타일 | [📄 보기](https://github.com/MM1152/CalmDown-Animals/blob/main/Assets/Scripts/Tiles/DrawTile.cs) |
| `InTileAnimal.cs` | 타일 내 동물 관리 (진입/퇴출 추적) | [📄 보기](https://github.com/MM1152/CalmDown-Animals/blob/main/Assets/Scripts/Tiles/InTileAnimal.cs) |
| `Tiles 폴더` | 전체 타일 관련 스크립트 | [📁 보기](https://github.com/MM1152/CalmDown-Animals/tree/main/Assets/Scripts/Tiles) |

---

### 🎮 게임소개

#### 🌿 **길**
- 동물들은 **사용자가 직접 그린 길**을 따라 이동합니다.
- **더 멀리** 돌아가게 만들어보세요!
- 맵이 **점점 커짐에 따라**, 다양한 길 그리기 **전략**을 고민해보세요.

#### 🦸 **대원**
- 웨이브를 진행하며 **강력한 대원**들을 구매하고 배치해보세요.
- 각 대원은 **특정 포획도구**를 사용해 흉포해진 동물 포획에 도전합니다.
- 포획도구마다 **포획 가능한 동물의 사이즈**가 다릅니다.
- 웨이브별 등장 동물을 파악해서 **최적의 포획도구**를 선택하세요.

#### 🐾 **동물**
- **동물마다 속도와 사이즈**가 달라 색다른 전략이 필요합니다.
- 등장 동물을 미리 파악해, **효율적으로 대원을 배치**하고 길을 그려보세요.

#### 📱 **간단한 플레이 방식**
- 첫 플레이는 **튜토리얼**을 통해 게임에 빠르게 익숙해질 수 있습니다.
- **터치와 드래그**만으로 누구나 쉽게 즐길 수 있는 **캐주얼 게임**입니다.

---

### 🎮 게임 설명

<table>
<tr>
<td align="center" width="50%">
   <img width="736" height="413" alt="Image" src="https://github.com/user-attachments/assets/ebe316e8-4775-486f-962c-a7c4cfd488c1" />
  <br>
  <sub>게임 메인화면/플레이 예시</sub>
</td>
<td align="center" width="50%">
  
 <img width="736" height="413" alt="Image" src="https://github.com/user-attachments/assets/ca5fafca-bb42-4800-841e-f97518cc4c87" />
  <br>
  <sub>맵 에디터 및 UI 예시</sub>
</td>
</tr>
</table>

#### ★ 주요 시스템 및 구조  
- **경로 생성 시스템** : A* 적용 실시간 경로 재계산, 육각 타일 그리드
- **동물 (적)** : 크기, 속도별로 구분되는 동물에 대한 내용 구현
- **대원 관리/전투** : CrewRank별 능력치, 장비 변경 시스템, 포획·전투 범위 표시
- **맵 에디터** : 인게임 맵 배치, 데이터 저장/불러오기
- **시각 효과** : 파티클/애니메이션/효과음 등 풍부한 인터랙션
- **UI/UX** : 팝업·점수·옵션창·튜토리얼 강화

---

### 🕹️ 플레이 방식

- **드래그, 클릭**으로 대원/타일/동물 조작
- 웨이브마다 다양한 동물 포획, 전투  
- `맵 에디터`를 통한 자유 맵 설계, 실제 게임 적용

---

### 🏗️ 구현 화면 예시


<table>
<tr>
  
<td align="center"><img src="https://github.com/user-attachments/assets/62177e66-b3ef-49a8-9ff7-b08a15dc8b36" height="300"/><br><sub>실시간 전투/포획</sub></td>
<td align="center"><img src="https://github.com/user-attachments/assets/42354fd9-2d85-4988-a163-fe0e43e36172" height="300"/><br><sub>대원 정보/무기 변경</sub></td>
</tr>
<tr>
<td align="center"><img src="https://github.com/user-attachments/assets/bd75fe59-50b2-478f-9284-98504af5b9e2" height="300"/><br><sub>길 그리기</sub></td>
<td align="center"><img src="https://github.com/user-attachments/assets/0f0e09f1-0128-434a-a2ec-bf9fc5d9434d" height="300"/><br><sub>대원 배치</sub></td>
</tr>
</table>


<div align="center">

<strong>Made with ❤️ by MM1152</strong>  
<strong>육각 타일 전략의 재미를 경험해보세요!</strong>  

</div>
