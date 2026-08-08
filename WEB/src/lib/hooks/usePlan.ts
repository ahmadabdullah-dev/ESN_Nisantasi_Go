import {  useQuery } from "@tanstack/react-query";
import agent from "../api/agent";
import type { PlanDto } from "../types/plan";
import type { PaginatedList, PaginationParams } from "../types/common";
export const usePlan = (pagination?: PaginationParams) => {
 
  const getPlansAsync = useQuery({
    queryKey: ["plans", pagination?.page, pagination?.pageSize],
    queryFn: async () =>
    await agent.get<PaginatedList<PlanDto>>("/Plan/paged", { params: pagination }).then((res) => res.data),
    enabled: !!pagination,
    staleTime: 5 * 60 * 1000,
  });

  return {
    getPlansAsync,
  };
};
