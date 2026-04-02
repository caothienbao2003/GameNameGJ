// using UnityEngine;

// public class MoveToGridPositionTeleport : MonoBehaviour, IMoveToPosition
// {
//     private IGridService _gridService;

//     private IGridService gridService
//     {
//         get
//         {
//             if(_gridService == null)
//             {
//                 _gridService = GetComponent(IGridService);
//             }

//             return _gridService;
//         }
//     }
    
//     public void SetMoveTargetPosition(Vector3 movePosition)
//     {
//         if (gridService == null)
//         {
//             Debug.LogWarning("[MoveToGridPositionTeleport] grid is null]");
//             return;
//         }
        
//         transform.position = gridService.RuntimeGrid.GetCellCenterWorldPos(movePosition);
//     }
// }
