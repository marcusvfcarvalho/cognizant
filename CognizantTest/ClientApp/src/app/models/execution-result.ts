import { ExecutionStatus } from "./execution-status";

export interface ExecutionResult {
  status: ExecutionStatus,
  output: string
}
