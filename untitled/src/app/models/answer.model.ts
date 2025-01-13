export interface AnswerDto {
  content: string;
  user: {
    id: string;
    username: string;
  };
  score: number;
  authorId: string;
}
