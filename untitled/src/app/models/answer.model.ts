export interface AnswerDto {
  content: string;
  user: {
    id: string;
    username: string;
  };
  score: number;
  authorId: string;    // The ID of the answer's author (redundant but still available)
}
