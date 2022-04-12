export type PersistedGrant = {
  key: string;
  type: string;
  subjectId: string;
  sessionId: string;
  clientId: string;
  description: string;
  creationTime: Date;
  expiration: Date;
  consumedTime: Date;
  Data: string;
};
