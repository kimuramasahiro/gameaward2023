using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Range(4, 100)]
    public int width;
    [Range(4, 100)]
    public int height;
    [SerializeField]
    RoomSettings roomSettings;

    [Serializable]
    public class RoomSettings
    {
        [Range(2, 10)]
        public int minWidth;
        [Range(2, 10)]
        public int minHeight;
        [Range(0, 100)]
        public int bigRoomRate;
        [Range(1, 10)]
        public int maxWallThicknessInArea;
    }
    [SerializeField]

    // �_���W�����}�b�v�𐶐����܂�
    public int[,] Generate()
    {
        int[,] map = new int[width, height];
        var baseArea = new Area(0, 0, width, height, roomSettings);
        // Area�𕪊�
        var dividedAreas = baseArea.Divide();
        // Area��`��
        foreach (var area in dividedAreas)
        {
            map = area.WriteToMap(map);
        }
        // Area���m���q���ʘH�����
        var passages = GeneratePassagesByArea(dividedAreas);
        // �ʘH��`��
        foreach (var passage in passages)
        {
            map = passage.WriteToMap(map);
        }

        return map;
    }

    // �G���A���q���ʘH�𐶐����܂�
    Passage[] GeneratePassagesByArea(Area[] areas)
    {
        // �אڂ����G���A���q����悤�ʘH�𐶐�
        var passages = new List<Passage>();
        foreach (var area1 in areas)
        {
            foreach (var area2 in areas)
            {
                if (area1 == area2 || !IsAdjacently(area1, area2))
                {
                    continue;
                }

                passages.Add(new Passage(area1, area2));
            }
        }

        // �s�v�ȒʘH�������Ă���
        var fixedPassages = new List<Passage>();
        while (passages.Count > 0)
        {
            // �ʘH�������_���łЂƂ폜
            var targetIndex = UnityEngine.Random.Range(0, passages.Count);
            var targetPassage = passages[targetIndex];
            passages.RemoveAt(targetIndex);

            // �S�G���A���q�����Ă��邩�`�F�b�N
            if (!IsAllAreaConnected(areas.ToList(), passages.ToArray(), fixedPassages.ToArray()))
            {
                // �폜�������ƂŃG���A���o�����Ă��܂����B�܂�����킯�ɂ͂����Ȃ��d�v�ȒʘH�Ȃ̂ŕێ�
                fixedPassages.Add(targetPassage);
            }
        }
        return fixedPassages.ToArray();
    }

    // �G���A���m���אڂ��Ă��邩�`�F�b�N���܂�
    bool IsAdjacently(Area area1, Area area2)
    {
        // Area�̈ʒu�֌W���`�F�b�N
        var left = area1.x < area2.x ? area1 : area2;
        var right = area1.x > area2.x ? area1 : area2;
        var top = area1.y > area2.y ? area1 : area2;
        var bottom = area1.y < area2.y ? area1 : area2;

        // ���E�ɐڂ��Ă��邩�ǂ����̃`�F�b�N
        if (null != left && null != right &&
            (left.x + left.width) == right.x &&
            (left.y <= right.y && right.y < (left.y + left.height) || right.y <= left.y && left.y < (right.y + right.height)))
        {
            return true;
        }

        // �㉺�ɐڂ��Ă��邩�ǂ����̃`�F�b�N
        if (null != top && null != bottom &&
            (bottom.y + bottom.height) == top.y &&
            (bottom.x <= top.x && top.x < (bottom.x + bottom.width) || top.x <= bottom.x && bottom.x < (top.x + top.width)))
        {
            return true;
        }

        return false;
    }

    // �S�ẴG���A���q�����Ă��邩�ǂ����`�F�b�N���܂�
    bool IsAllAreaConnected(List<Area> areas, Passage[] passages1, Passage[] passages2)
    {
        if (areas.Count <= 1)
        {
            return true;
        }

        var passages = new List<Passage>();
        passages.AddRange(passages1);
        passages.AddRange(passages2);

        // �G���A[0]���`�F�b�N�ΏۂƂ��A�`�F�b�N�J�n
        var checkingAreas = new List<Area>() { areas[0] };
        areas.RemoveAt(0);
        var checkedAreas = new List<Area>() { };

        while (checkingAreas.Count > 0)
        {
            var nextCheckTargetAreas = new List<Area>() { };
            foreach (var checkTargetArea in checkingAreas)
            {
                // �`�F�b�N�Ώۂ̃G���A����L�т�ʘH���擾
                foreach (var passage in passages.Where(x => x.areas.Contains(checkTargetArea)))
                {
                    // �`�F�b�N�Ώۂ̃G���A����A�ʘH�łȂ��ꂽ�G���A���擾
                    var pairedArea = passage.areas.First(x => x != checkTargetArea);
                    if (!checkedAreas.Contains(pairedArea) && !checkingAreas.Contains(pairedArea) && !nextCheckTargetAreas.Contains(pairedArea))
                    {
                        // �ʘH�łȂ��ꂽ�G���A��areas���珜���A����̃`�F�b�N�ΏۃG���A�ɂ���
                        areas.Remove(pairedArea);
                        nextCheckTargetAreas.Add(pairedArea);
                    }
                }
            }
            checkedAreas.AddRange(checkingAreas);
            checkingAreas = nextCheckTargetAreas;
        }

        // areas����S�ẴG���A���������ꂽ�Ȃ�΁A�S�ẴG���A���q�����Ă���Ƃ������ƂɂȂ�
        return areas.Count == 0;
    }

    // �G���A�N���X
    class Area
    {
        public readonly int x;
        public readonly int y;
        public readonly int width;
        public readonly int height;
        public readonly RoomSettings roomSettings;
        public readonly Room room;

        // �G���A�𕪊��\���ǂ���
        bool IsDividable
        {
            get { return IsDividableHorizontal || IsDividableVertical; }
        }

        // �G���A�����ɕ����\���ǂ���
        bool IsDividableHorizontal
        {
            get { return MinWidth * 2 <= width; }
        }

        // �G���A���c�ɕ����\���ǂ���
        bool IsDividableVertical
        {
            get { return MinHeight * 2 <= height; }
        }

        // �G���A���̍ŏ��l
        int MinWidth
        {
            get { return roomSettings.minWidth + 2; }
        }

        // �G���A�����̍ő�l
        int MinHeight
        {
            get { return roomSettings.minHeight + 2; }
        }

        // �R���X�g���N�^
        public Area(int x, int y, int width, int height, RoomSettings roomSettings)
        {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.roomSettings = roomSettings;
            this.room = GenerateRoom();
        }

        // �G���A�𕪊����܂�
        public Area[] Divide()
        {
            var dividableAreas = new Area[] { this };
            var devidedAreas = new List<Area>();
            var fixedAreas = new List<Area>{ };

            // Area�������J��Ԃ�
            while (true)
            {
                // �����s�\�ȃG���A��fixed�ɓ����
                fixedAreas.AddRange(dividableAreas.Where(x => !x.IsDividable));

                if (dividableAreas.Length == 0)
                {
                    // �����\�ȃG���A�������Ȃ烋�[�v�𔲂���
                    break;
                }

                devidedAreas.Clear();
                // �����\�ȃG���A�͕��������݂�
                foreach (var area in dividableAreas.Where(x => x.IsDividable))
                {
                    if (UnityEngine.Random.Range(0, 100) < roomSettings.bigRoomRate)
                    {
                        // ������x�����𕪊��ς݂̎��A���m���ŃG���A�𕪊��������̂܂ܕ����ɂ���
                        fixedAreas.Add(area);
                    }
                    else
                    {
                        devidedAreas.AddRange(area.DivideOnceIfPossible());
                    }
                }
                dividableAreas = devidedAreas.ToArray();
            }

            return fixedAreas.ToArray();
        }

        // �}�b�v�ɕ������������݂܂�
        public int[,] WriteToMap(int[,] map)
        {
            for (int dx = room.x; dx < room.x + room.width; dx++)
            {
                for (int dy = room.y; dy < room.y + room.height; dy++)
                {
                    map[dx, dy] = 1;
                }
            }
            return map;
        }

        // �G���A���ɕ����𐶐����܂�
        Room GenerateRoom()
        {
            var left = UnityEngine.Random.Range(1, Math.Min(1 + roomSettings.maxWallThicknessInArea, width - roomSettings.minWidth));
            var right = UnityEngine.Random.Range(Math.Max(width - roomSettings.maxWallThicknessInArea, left + roomSettings.minWidth), width - 1);
            var bottom = UnityEngine.Random.Range(1, Math.Min(1 + roomSettings.maxWallThicknessInArea, height - roomSettings.minHeight));
            var top = UnityEngine.Random.Range(Math.Max(height - roomSettings.maxWallThicknessInArea, bottom + roomSettings.minHeight), height - 1);
            return new Room(x + left, y + bottom, right - left, top - bottom);
        }

        // �\�ł���΃G���A��1�񂾂��������܂�
        Area[] DivideOnceIfPossible()
        {
            if (IsDividableHorizontal && IsDividableVertical && UnityEngine.Random.Range(0, 2) == 0 || IsDividableHorizontal && !IsDividableVertical)
            {
                // ���E�ɕ���
                var dividePosX = UnityEngine.Random.Range(x + MinWidth, x + width - MinWidth + 1);
                return new Area[]
                {
                    new Area(x, y, dividePosX - x, height, roomSettings),
                    new Area(dividePosX, y, width - (dividePosX - x), height, roomSettings)
                };
            }
            else if (IsDividableVertical)
            {
                // �㉺�ɕ���
                var dividePosY = UnityEngine.Random.Range(y + MinHeight, y + height - MinHeight + 1);
                return new Area[]
                {
                    new Area(x, y, width, dividePosY - y, roomSettings),
                    new Area(x, dividePosY, width, height - (dividePosY - y), roomSettings)
                };
            }
            else
            {
                // �����s�\�Ȃ炻�̂܂ܕԂ�
                return new Area[] { this };
            }
        }

        // �����N���X
        public class Room
        {
            public readonly int x;
            public readonly int y;
            public readonly int width;
            public readonly int height;

            public Room(int x, int y, int width, int height)
            {
                this.x = x;
                this.y = y;
                this.width = width;
                this.height = height;
            }
        }
    }

    // �ʘH�N���X
    class Passage
    {
        // �ʘH�Ōq���G���A
        public readonly Area[] areas;

        public Passage(Area area1, Area area2)
        {
            this.areas = new Area[] { area1, area2 };
        }

        // �}�b�v�ɒʘH���������݂܂�
        public int[,] WriteToMap(int[,] map)
        {
            var fromX = UnityEngine.Random.Range(areas[0].room.x, areas[0].room.x + areas[0].room.width);
            var fromY = UnityEngine.Random.Range(areas[0].room.y, areas[0].room.y + areas[0].room.height);
            var toX = UnityEngine.Random.Range(areas[1].room.x, areas[1].room.x + areas[1].room.width);
            var toY = UnityEngine.Random.Range(areas[1].room.y, areas[1].room.y + areas[1].room.height);
            while (fromX != toX || fromY != toY)
            {
                //�ύX��---------------------------------------------------------
                //map[x,y]�̃p�����^�F0:�ǁA1:�����A2:�ʘH�A10:�v���C���[�����镔��
                if (map[fromX, fromY] == 0)
                {
                    map[fromX, fromY] = 2;
                }
                else
                {
                    map[fromX, fromY] = 1;
                }
                //�ύX��---------------------------------------------------------

                if (fromX != toX && fromY != toY && UnityEngine.Random.Range(0, 2) == 0 || fromY == toY)
                {
                    fromX += (toX - fromX) > 0 ? 1 : -1;
                }
                else
                {
                    fromY += (toY - fromY) > 0 ? 1 : -1;
                }
            }
            return map;
        }
    }
}