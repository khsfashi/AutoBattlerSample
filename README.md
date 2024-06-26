<p align="center">
  <img src="https://github.com/khsfashi/AutoBattlerSample/assets/55976865/e393413a-0ce5-40df-8f4a-9f9fcd5ed6ab">
</p>


# Auto Battler Sample
Auto Battler 장르의 게임을 제작하기 위한 샘플입니다.
2D 기반으로 타일 맵 위에서 유닛을 배치하고 자동으로 전투하게 합니다.
유니티를 통해 게임을 개발해본 경험이 없는 개발자가
2024.05.05 ~ 2024.05.10의 기간 동안 개발한 Sample 프로젝트입니다.

# APK 다운로드 링크
[Google Drive](https://drive.google.com/file/d/1rnVWNNUdVPZO7p0AjeiO36-PEiwT2ogC/view?usp=drive_link)

# Youtube 설명 영상
[Youtube](https://www.youtube.com/watch?v=W8V9LDE2Cvk&t=1s)

# 구현된 기능
* 타이틀 화면
  - 시작 버튼 클릭 시 화면 페이드 인/아웃
* 화면 클릭 시 터치 이펙트
  - C# 스크립트로 UI 이미지에 애니메이션 효과 적용
* 5 x 10 타일 맵
* 노드와 그래프로 각 타일을 구성하여 최단 경로 탐색 (다익스트라 알고리즘)
* 경로가 없을 시 주변 빈 타일 중 적과 가까운 타일을 선택하여 이동
* 세가지 유닛 존재 (근접 - Knight, 광역 - Samurai, 원거리 - Archer)
* 각 유닛을 구매하고 새로고침 할 수 있는 상점 존재 (TFT와 동일)
* 돈을 주고 사용하는 버프(꾹 누를 시 설명 툴팁)
* 타일 맵 위 유닛 이동 배치 가능
* 자동 전투
* 제한 시간 및 라운드(라운드마다 적 유닛 수 증가)
* NPC와 상황에 따른 대사
