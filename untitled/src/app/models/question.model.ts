export interface Question {
  answers: any[];
  questionId: string;
  content: string;
  score: number;
  viewCount: number;
  viewsCount: number;
  userId: string;
  crtVote: number;

  userVote?: number;
}
