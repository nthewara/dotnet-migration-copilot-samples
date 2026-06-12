# 04-final-validation-and-handoff: Validate upgraded solution and document follow-up

Run final validation across the upgraded solution after the side-by-side migration task completes. This includes restore, full solution build, available tests, and review of assessment success criteria for `net10.0` readiness. The task should also document any intentional post-upgrade follow-up, especially that the agent does not delete the old Framework project as part of this migration.

This final task confirms that the new ASP.NET Core `net10.0` project is the validated migration target and that the existing Framework project remains available for comparison or production cutover decisions. It should capture any remaining manual verification or deployment/cutover actions for the user.

**Done when**: Restore/build/test validation succeeds with zero errors and warnings for modified projects, tasks are complete, any deferred or manual follow-up is documented, and the upgrade state is ready for user review and source-control commit according to the Single Commit at End strategy.
