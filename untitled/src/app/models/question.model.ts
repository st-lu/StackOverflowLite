export interface Question {
  answers: any[];
  questionId: string;
  content: string;
  score: number;
  viewsCount: number;
  userId: string;

  userVote?: number; // -1 for downvote, 0 for neutral, 1 for upvote
}
