namespace BlackJackGameLogic
{
    public class Class1
    {

        <xsl:template match="Citation">
    <div class="pn-poms-citation">
        <span class="pn-poms-citation-title pn-poms-bold">
            CITATIONS:
        </span>
        <span class="pn-poms-citation-text">
            <xsl:apply-templates/>
        </span>
    </div>
</xsl:template>



    }
}
