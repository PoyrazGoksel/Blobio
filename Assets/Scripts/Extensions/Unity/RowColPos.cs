using System.Collections.Generic;
using Extensions.System;
using UnityEngine;

namespace Extensions.Unity
{
    internal struct RowColPos
    {
        public const int MaxRowCount = 20;
        public static readonly RowColPos zero = new RowColPos(0, 0);

        public int R
        {
            get => _r;
            set => _r = value;
        }

        public int C
        {
            get => _c;
            set => _c = value;
        }

        private int _r;
        private int _c;

        public RowColPos(int row, int column)
        {
            _r = row;
            _c = column;
        }

        public RowColPos(int bulletIndex)
        {
            _c = bulletIndex / MaxRowCount;
            _r = bulletIndex % MaxRowCount;
        }

        public RowColPos GetRowColPos(int bulletIndex)
        {
                
            int bulletCount = bulletIndex + 1;
                
            C = bulletCount / MaxRowCount;
            R = bulletCount % MaxRowCount;
            return this;
        }

        public int MaxRowIndexAt(int c)
        {
            if (c == _c)
            {
                return _r;
            }
            else
            {
                return MaxRowCount.ToIndex();
            }
        }
            
        public int MaxColIndexAt(int r)
        {
            if (r > _r)
            {
                return _c - 1;
            }
            else
            {
                return _c;
            }
        }
            
        public int LastIndex()
        {
            return ((_c * MaxRowCount) + _r);
        }

        public static int ParseToIndex(int r, int c)
        {
            return ((c * MaxRowCount) + r);
        }
        
        public static Vector2Int ParseToRowCol(int index, int width)
        {
            Vector2Int pos = Vector2Int.zero;
            int bulletCount = index + 1;
                
            pos.y = bulletCount / width;
            pos.x = bulletCount % width;
            
            return pos;
        }

        private static int Count(int r, int c)
        {
            return ParseToIndex(r, c) + 1;
        }

        private int Count()
        {
            return LastIndex() + 1;
        }

        public Vector3 GetBulletLocPos(Vector3 stackPaddingData)
        {
            float newColumnYOffset = C * stackPaddingData.y / 2f;

            Vector3 newPos = Vector3.zero;

            newPos.y = stackPaddingData.y * R;
            newPos.z = stackPaddingData.z * C;
            newPos.y += newColumnYOffset;
            return newPos;
        }

        public static List<T> OrderByCol<T>(List<T> bulletsToGive)
        {
            List<T> bulletsToMove = new List<T>();

            RowColPos lastRowColPos = new RowColPos(bulletsToGive.Count.ToIndex());
            
            for (int col = 0; col <= lastRowColPos.C; col++)
            {
                if (col == lastRowColPos.C)
                {
                    for (int lastRowIndices = 0; lastRowIndices <= lastRowColPos.R; lastRowIndices++)
                    {
                        bulletsToMove.Add(bulletsToGive[ParseToIndex(lastRowIndices, col)]);
                    }
                    
                    break;
                }
                
                for (int row = 0; row < MaxRowCount; row++)
                { 
                    bulletsToMove.Add(bulletsToGive[ParseToIndex(row, col)]);
                }
            }
                
            return bulletsToMove;
        }
            
        public static List<T> OrderByRow<T>(List<T> bulletsToGive)
        {
            List<T> bulletsToMove = new List<T>();

            RowColPos lastRowColPos = new RowColPos(bulletsToGive.Count.ToIndex());
                
            for (int row = 0; row < MaxRowCount; row++)
            {
                for (int col = 0; col <= lastRowColPos.MaxColIndexAt(row); col++)
                {
                    bulletsToMove.Add(bulletsToGive[ParseToIndex(row, col)]);
                }
            }
                
            return bulletsToMove;
        }
    }
}