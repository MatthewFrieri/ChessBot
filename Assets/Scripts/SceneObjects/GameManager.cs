using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject pawnWhite;
    public GameObject rookWhite;
    public GameObject knightWhite;
    public GameObject bishopWhite;
    public GameObject queenWhite;
    public GameObject kingWhite;

    public GameObject pawnBlack;
    public GameObject rookBlack;
    public GameObject knightBlack;
    public GameObject bishopBlack;
    public GameObject queenBlack;
    public GameObject kingBlack;

    public Dictionary<int, GameObject> pieceToGameObject = new Dictionary<int, GameObject>();

    public GameObject captureIndicator;
    public GameObject moveIndicator;

    private AudioSource audioSource;
    public AudioClip moveAudioClip;
    public AudioClip captureAudioClip;
    public AudioClip checkAudioClip;

    private void Start()
    {

        audioSource = GetComponent<AudioSource>();

        pieceToGameObject[Piece.Pawn | Piece.White] = pawnWhite;
        pieceToGameObject[Piece.Rook | Piece.White] = rookWhite;
        pieceToGameObject[Piece.Knight | Piece.White] = knightWhite;
        pieceToGameObject[Piece.Bishop | Piece.White] = bishopWhite;
        pieceToGameObject[Piece.Queen | Piece.White] = queenWhite;
        pieceToGameObject[Piece.King | Piece.White] = kingWhite;
        pieceToGameObject[Piece.Pawn | Piece.Black] = pawnBlack;
        pieceToGameObject[Piece.Rook | Piece.Black] = rookBlack;
        pieceToGameObject[Piece.Knight | Piece.Black] = knightBlack;
        pieceToGameObject[Piece.Bishop | Piece.Black] = bishopBlack;
        pieceToGameObject[Piece.Queen | Piece.Black] = queenBlack;
        pieceToGameObject[Piece.King | Piece.Black] = kingBlack;


        string pgn = "1. d4 d5 2. e4 e6 3. e5 g6 4. g4 a5 5. Qf3 b5 6. Qh3 b4 7. Qxh7 Nc6 8. Ba6 Rxa6 9. Nf3 Ra8 10. O-O Qh4 11. Ng5 Qxf2+ 12. Kh1 Qxc2 13. Ne4 Qxb1 14. Nd6+ Ke7 15. Bg5+ Kd7 16. Qh6 Nf6 17. Qxf8 Nd8 18. Qh6 Ne4 19. Qh5 Nd2 20. Qh3 Ne4 21. Qc3 Nd2 22. Nb5 Ke8 23. Rg1 Kf8 24. Rf1 Kg7 25. Qxc7 Kh7 26. Nd6 Ne4 27. Qc6 Nc3 28. Qc7 Nb5 29. Qc6 Na7 30. Qc7 Nac6 31. Qxc8 Qc2 32. Qb7 Qc5 33. Qb5 Qb6 34. Qe2 Qb5 35. Qe3 Qb6 36. Qh3+ Kg7 37. Rae1 Qb5 38. Re2 Qb6 39. Ref2 Qb5 40. Qg3 Qb6 41. Qg2 Qb5 42. Qh3 Qb6 43. Kg1 Qc5 44. Nc4 Qb6 45. Nd6 Qb5 46. Nc4 Qc5 47. Nd6 Qb6 48. Rxf7+";
        pgn = "1. d4 d5 2. e4 e6 3. e5 g6 4. g4 a5 5. Qf3 b5 6. Qh3 b4 7. Qxh7 Nc6 8. Ba6 Rxa6 9. Nf3 Ra8 10. O-O Qh4 11. Ng5 Qxf2+ 12. Kh1 Qxc2 13. Ne4 Qxb1 14. Nd6+ Ke7 15. Bg5+ Kd7 16. Qh6 Nf6 17. Qxf8 Nd8 18. Qh6 Ne4 19. Qh5 Nd2 20. Qh3 Ne4 21. Qc3 Nd2 22. Nb5 Ke8 23. Rg1 Kf8 24. Rf1 Kg7 25. Qxc7 Kh7 26. Nd6 Ne4 27. Qc6 Nc3 28. Qc7 Nb5 29. Qc6 Na7 30. Qc7 Nac6 31. Qxc8 Qc2 32. Qb7 Qc5 33. Qb5 Qb6 34. Qe2 Qb5 35. Qe3 Qb6 36. Qh3+ Kg7 37. Rae1 Qb5 38. Re2 Qb6 39. Ref2 Qb5 40. Qg3 Qb6 41. Qg2 Qb5 42. Qh3 Qb6 43. Kg1 Qc5 44. Nc4 Qb6 45. Nd6 Qb5 46. Nc4 Qc5 47. Nd6 Qb6 48. Rxf7+ Nxf7 49. Rxf7+";
        Game.Init(Piece.White, pieceToGameObject, pgn);
    }


    public void PlayMoveSound()
    {
        audioSource.clip = moveAudioClip;
        audioSource.Play();
    }
    public void PlayCaptureSound()
    {
        audioSource.clip = captureAudioClip;
        audioSource.Play();
    }
    public void PlayCheckSound()
    {
        audioSource.clip = checkAudioClip;
        audioSource.Play();
    }
}
