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

export type ServerSideSession = {
  key: string;
  scheme: string;
  subjectId: string;
  sessionId: string;
  displayName: string;
  created: number;
  renewed: number;
  expires: number;
};

export type ServerSideSessionsQueryResult = {
  resultsToken: string;
  hasPrevResults: boolean;
  hasNextResults: boolean;
  totalCount: number;
  totalPages: number;
  currentPage: number;
  results: ServerSideSession[];
};
