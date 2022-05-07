export type PersistedGrant = {
  id: string;
  key: string;
  type: string;
  subjectId: string;
  sessionId: string;
  clientId: string;
  description: string;
  creationTime: number;
  expiration: number;
  consumedTime: number;
  data: string;
};
