export type UserSession = {
  id: string;
  key: string;
  subjectId: string;
  sessionId?: string;
  created: number;
  renewed: number;
  expires: number;
  ticket: string;
  applicationName: string;
};
